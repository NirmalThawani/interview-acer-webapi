USE [InterviewAcer]
GO
/****** Object:  Table [dbo].[InterviewCompletedStageMapping]    Script Date: 8/3/2018 1:59:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[InterviewCompletedStageMapping](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[InterviewId] [int] NOT NULL,
	[StageId] [int] NOT NULL,
 CONSTRAINT [PK_InterviewCompletedStageMapping] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[InterviewStageFeedback]    Script Date: 8/3/2018 1:59:26 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[InterviewStageFeedback](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[InterviewId] [int] NOT NULL,
	[StageId] [int] NOT NULL,
	[Feedback] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_InterviewStageFeedback] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
ALTER TABLE [dbo].[InterviewCompletedStageMapping]  WITH CHECK ADD  CONSTRAINT [FK_InterviewCompletedStageMapping_InterviewDetails] FOREIGN KEY([InterviewId])
REFERENCES [dbo].[InterviewDetails] ([InterviewDetailId])
GO
ALTER TABLE [dbo].[InterviewCompletedStageMapping] CHECK CONSTRAINT [FK_InterviewCompletedStageMapping_InterviewDetails]
GO
ALTER TABLE [dbo].[InterviewCompletedStageMapping]  WITH CHECK ADD  CONSTRAINT [FK_InterviewCompletedStageMapping_Stages] FOREIGN KEY([StageId])
REFERENCES [dbo].[Stages] ([Id])
GO
ALTER TABLE [dbo].[InterviewCompletedStageMapping] CHECK CONSTRAINT [FK_InterviewCompletedStageMapping_Stages]
GO
ALTER TABLE [dbo].[InterviewStageFeedback]  WITH CHECK ADD  CONSTRAINT [FK_InterviewStageFeedback_InterviewDetails] FOREIGN KEY([InterviewId])
REFERENCES [dbo].[InterviewDetails] ([InterviewDetailId])
GO
ALTER TABLE [dbo].[InterviewStageFeedback] CHECK CONSTRAINT [FK_InterviewStageFeedback_InterviewDetails]
GO
ALTER TABLE [dbo].[InterviewStageFeedback]  WITH CHECK ADD  CONSTRAINT [FK_InterviewStageFeedback_Stages] FOREIGN KEY([StageId])
REFERENCES [dbo].[Stages] ([Id])
GO
ALTER TABLE [dbo].[InterviewStageFeedback] CHECK CONSTRAINT [FK_InterviewStageFeedback_Stages]
GO
/****** Object:  StoredProcedure [dbo].[usp_UpdateCheckList]    Script Date: 8/3/2018 1:59:26 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[usp_UpdateCheckList]
	@checklistId int, @interviewId int
AS
BEGIN
	insert into InterviewCheckListMapping values(@interviewId, @checklistId);
	select s.Id As StageId, COUNT(gcl.Id) CompletedCheckList into #temp from Stages s
	inner join StageGroups sg on s.Id = sg.StageId
	inner join GroupCheckList gcl on sg.Id = gcl.GroupId
	inner join InterviewCheckListMapping iclm on iclm.CheckListId = gcl.Id
	where iclm.InterviewId = @interviewId
	group by s.Id

	select s.Id As StageId, COUNT(gcl.Id) totalchecklist into #temp2  from Stages s
	inner join StageGroups sg on s.Id = sg.StageId
	inner join GroupCheckList gcl on sg.Id = gcl.GroupId
	group by s.Id

	INSERT INTO InterviewCompletedStageMapping  
	SELECT @interviewId, #temp.StageId from #temp
	inner join #temp2 on #temp.CompletedCheckList = #temp2.totalchecklist and #temp.StageId = #temp2.StageId 

	--UPDATE STAGES SET IsCompleted = 1 where 
	--Id in ( select distinct #temp.StageId from #temp
	--inner join #temp2 on #temp.CompletedCheckList = #temp2.totalchecklist and #temp.StageId = #temp2.StageId )

	select top 1 Id from Stages s 
	WHERE S.Id not in (select StageId from InterviewCompletedStageMapping where InterviewId = @interviewId)
	AND S.InterviewTypeId = (select InterviewTypeId from InterviewDetails where InterviewDetailId = @interviewId)
	order by Sequence

	IF OBJECT_ID('tempdb..#temp') IS NOT NULL
  /*Then it exists*/
  DROP TABLE #temp

  
	IF OBJECT_ID('tempdb..#temp2') IS NOT NULL
  /*Then it exists*/
  DROP TABLE #temp2

END

GO
