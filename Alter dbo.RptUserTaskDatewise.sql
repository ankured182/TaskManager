USE [TaskManagerDB]
GO

/****** Object: SqlProcedure [dbo].[RptUserTaskDatewise] Script Date: 6/23/2022 2:34:06 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




--EXEC RptUserTaskDatewise '06/13/2022','06/21/2022'
ALTER PROC RptUserTaskDatewise
@df varchar(50),@dt varchar(50)
as
begin
 
 DECLARE @temp table (Id  int identity(1,1),UserName varchar(100),Assigned int,Pending int,Completed int,TentDurationHrsC varchar(500),ActualDuration varchar(100) ,TentDurationHrsP varchar(500),StartedNum int )
 INSERT @temp (UserName,Assigned,Pending,Completed,TentDurationHrsC,ActualDuration,TentDurationHrsP,StartedNum)
 SELECT DISTINCT UserName,0,0,0,0,0,0,0 from AspNetUsers
 --select * FROM @temp

 DECLARE @ctr int=1
 DECLARE @ctrMax int=(SELECT Count(*) FROM @temp)

 DECLARE @assigned int=0,@pending int=0,@completed int=0,@tentDur float=0,@actDur varchar(200),@thisUser varchar(50),@pendingDur float=0,@startedNum int

 WHILE (@ctr<=@ctrMax)
 BEGIN
      set @thisUser=(SELECT UserName FROM @temp WHERE Id=@ctr)   
      
      set @assigned=(select COUNT(Id) FROM TaskMains where           AssignedTo=@thisUser AND CONVERT(date,(DateCreated)) between @df and @dt)  
      set @pending=(select COUNT(Id) FROM TaskMains  where           AssignedTo=@thisUser AND CONVERT(date,(DateCreated)) between @df and @dt  AND CurrentStatus<>'Completed')  

      set @completed=(select COUNT(Id) FROM TaskMains where          AssignedTo=@thisUser AND CONVERT(date,(ActualDateEnded)) between @df and @dt  AND CurrentStatus='Completed')  
     
      set @tentDur=ISNULL((select SUM(DurationHrs) FROM TaskMains where     AssignedTo=@thisUser AND CONVERT(date,(ActualDateEnded)) between @df and @dt  AND CurrentStatus='Completed') ,'0')     
      set @actDur='0'
      set @pendingDur=ISNULL((select SUM(DurationHrs) FROM TaskMains where  AssignedTo=@thisUser AND CONVERT(date,(DateCreated)) between @df and @dt  AND CurrentStatus<>'Completed') ,'0')  
      set @startedNum=ISNULL((select COUNT(Id) FROM TaskMains        where  AssignedTo=@thisUser AND CONVERT(date,(ActualDateStarted)) between @df and @dt  AND CurrentStatus='Started'),'0') 
      
      UPDATE @temp SET  UserName=@thisUser,Assigned=@assigned,Pending=@pending,Completed=@completed,TentDurationHrsC=@tentDur,ActualDuration=@actDur,TentDurationHrsP=@pendingDur,StartedNum=@startedNum    WHERE Id=@ctr

      SET @ctr=@ctr+1
 END
 truncate table RptTasks

 --INSERT RptTasks(UserName,Assigned,Pending,Completed,TentDurationHrsC,ActualDuration,TentDurationHrsP,StartedNum)
 --select UserName,Assigned,Pending,Completed,TentDurationHrsC,ActualDuration,TentDurationHrsP,StartedNum FROM @temp

  select Id,UserName,Assigned,Pending,Completed,StartedNum,TentDurationHrsC,ActualDuration,TentDurationHrsP FROM @temp

end

--select CAST((ActualDateEnded-ActualDateStarted) ) as time(0) )  FROM TaskMains where  CONVERT(date,(ActualDateEnded)) between '06/13/2022' AND '06/21/2022'  AND CurrentStatus='Ended'

--SELECT CAST((@dt2-@dt1) as time(0))

--DECLARE  @StartDate datetime = '10/01/2012 08:40:18.000'
--        ,@EndDate   datetime = '10/04/2012 09:52:48.000'

--SELECT
--    STR(ss/3600, 5) + ':' + RIGHT('0' + LTRIM(ss%3600/60), 2) + ':' + RIGHT('0' + LTRIM(ss%60), 2) AS [hh:mm:ss]
--FROM (VALUES(DATEDIFF(s, @StartDate, @EndDate))) seconds (ss)
