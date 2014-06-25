using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System;
using System.Collections.Generic;

namespace LineService
{
    public class ProductsToMove : Queue<IStation>  
    {

        public event EventHandler NewItemsCame;
        new public void Enqueue(IStation station) 
        {
            base.Enqueue(station);
            if(this.NewItemsCame != null)
                this.NewItemsCame(this, new EventArgs());
        }
    }




    /// <summary>
    /// Статический класс, предназначенный для организации собственной очереди обработки событий.
    /// </summary>
    /// <typeparam name="T">Тип объектов, которые будут переданы обрабочикам событий в 
    /// параметре sender.</typeparam>
    public static class EventQueue<T> where T : class
    {
        /// <summary>
        /// Режим работы очереди событий: 
        /// <value>Queue</value> (режим очереди): новое событие будет обработано последним, 
        /// <value>Stack</value> (режим стека): новое событие будет обработано первым, 
        /// <value>Immediate</value> (классический режим): новое событие будет вызвано немедлено.
        /// </summary>
        public enum EventQueueMode
        {
            /// <summary>
            /// Режим очереди: новое событие будет обработано последним.
            /// </summary>
            Queue,
            /// <summary>
            /// Режим стека: новое событие будет обработано первым.
            /// </summary>
            Stack,
            /// <summary>
            /// Классический режим: новое событие будет вызвано немедлено.
            /// </summary>
            Immediate
        }

        /// <summary>
        /// Класс, являющийся элементом очереди событий.
        /// </summary>
        public sealed class Event : IEquatable<Event>
        {
            private readonly Delegate[] handlers;
            private readonly T sender;
            private readonly EventArgs args;

            public Event(Delegate Handler, T Sender, EventArgs Args)
                : this(Handler == null ? null : Handler.GetInvocationList(), Sender, Args)
            {
            }

            internal Event(Delegate[] Handlers, T Sender, EventArgs Args)
            {
                if (Handlers == null)
                    throw new ArgumentNullException("Handlers");
                handlers = Handlers;
                sender = Sender;
                args = Args;
            }

            internal void Invoke()
            {
                if (Sorter != null && handlers.Length > 1)
                    Array.Sort(handlers, Sorter);
                foreach (Delegate d in handlers)
                    d.DynamicInvoke(new object[] {sender, args});
            }

            public bool Equals(Event other)
            {
                return handlers == other.handlers && sender == other.sender && args == other.args;
            }
        }

        /// <summary>
        /// Если значение данного поля не null, то обработчики события перед вызовом будут 
        /// отсортированы при помощи делегата, указанного в данном поле.
        /// </summary>
        public static Comparison<Delegate> Sorter;

        private static bool CanRun = true;
        private static bool running;
        private static readonly LinkedList<Event> events = new LinkedList<Event>();

        private static EventQueueMode queueMode;
        private static Action<Event> addFunctor;

        static EventQueue()
        {
            QueueMode = EventQueueMode.Queue;
        }

        /// <summary>
        /// Добавить новое событие в очередь.
        /// </summary>
        /// <param name="Handler">Событие (event).</param>
        /// <param name="Sender">Объект, вызвавший событие.</param>
        /// <param name="Args">Параметры события.</param>
        public static void Add(Delegate Handler, T Sender, EventArgs Args)
        {
            Add(new Event(Handler, Sender, Args));
        }

        /// <summary>
        /// Добавить новое событие в очередь.
        /// </summary>
        /// <param name="newEvent">Событие (EventPool.Event).</param>
        public static void Add(Event newEvent)
        {
            if (events.Contains(newEvent))
                return;
            addFunctor(newEvent);
            if (CanRun && events.Count == 1 && !running)  // if (CanRun && events.Count == 1 && !running)
                Resume();
        }

        /// <summary>
        /// Приостановить вызовы.
        /// </summary>
        public static void Suspend()
        {
            CanRun = false;
        }

        /// <summary>
        /// Возобновить вызовы.
        /// </summary>
        public static void Resume()
        {
            CanRun = true;
            while (events.Count > 0 && CanRun)
            {
                try
                {
                    Event e = events.First.Value;
                    running = true;
                    e.Invoke();
                    running = false;
                    events.RemoveFirst();

                }
                catch (Exception ex) 
                {
                    Console.WriteLine("Events.Count = " + events.Count.ToString());
                    Console.WriteLine(ex.ToString());
                    events.RemoveFirst();
                }
            }
        }

        /// <summary>
        /// Возвращает количество событий, находящихся в очереди.
        /// </summary>
        public static int Count
        {
            get { return events.Count; }
        }

        /// <summary>
        /// Возвращает/устанавливает режим работы очереди обработчиков событий. 
        /// Значение по-умолчанию: <value>Queue</value>.
        /// См. <see cref="EventQueueMode"/>.
        /// </summary>
        /// <exception cref="ApplicationException">Если при установке режима 
        /// <see cref="EventQueueMode.Immediate"/> очередь событий не пуста.</exception>
        public static EventQueueMode QueueMode
        {
            get
            {
                return queueMode;
            }
            set
            {
                queueMode = value;
                switch (value)
                {
                    case EventQueueMode.Immediate:
                        if (events.Count != 0)
                            throw new ApplicationException("Event queue is not empty!");
                        addFunctor = e => e.Invoke();
                        break;
                    case EventQueueMode.Queue:
                        addFunctor = e => events.AddLast(e);
                        break;
                    case EventQueueMode.Stack:
                        addFunctor = e => events.AddFirst(e);
                        break;
                }
            }
        }
    }

}
