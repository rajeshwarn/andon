c

alter table Parts 
	add Address varchar(100) null
go

update Parts
	set Address = ''
go

select * from Parts
go


select * from Control
go



SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER up_Parts_Control  ON  Parts  AFTER UPDATE
AS BEGIN
	SET NOCOUNT ON;
    declare @id int;
    declare @partName varchar(255);
    
    declare inserted_rows cursor 
    for select i.Id, i.Name
    from inserted i;
    
    open inserted_rows;
    fetch next from inserted_rows
    into @id, @partName;
    
    while @@FETCH_STATUS = 0
	begin
		update Control
		set KeyString = @partName
		where PartId = @id      
        
        fetch next from inserted_rows
		into @id, @partName;
	end;
END
GO


