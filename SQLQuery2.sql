

DROP TABLE [dbo].[RptTasks]

CREATE TABLE [dbo].[RptTasks] (
    [Id]               INT           IDENTITY (1, 1) NOT NULL,
    [UserName]         NVARCHAR (50) NULL,
    [Assigned]         INT           NULL,
    [Pending]          INT           NULL,
    [Completed]        INT           NULL,
    [StartedNum]       INT           NULL,
    [TentDurationHrsC] NVARCHAR(500)    NULL,
    [ActualDuration] VARCHAR(500) NULL,
    [TentDurationHrsP] NVARCHAR(500)    NULL
);
