/****** SSMS 中 SelectTopNRows 命令的指令碼  ******/
SELECT TOP (1000) 
case sp.Class when 1 then '起漲放量'
  when 5 then '起漲五倍量'
  when 10 then '起漲十倍量'
  when 6 then '盤月放量'
  when 7 then '盤季放量'
  when 8 then '半年放量'
  when 9 then '盤年放量'
end as '型態',
      sp.[Date] as 'StartDate' 
      ,sp.[Code]
	  ,sg.Name
      ,[StartPrice]
	  ,sr.ClosingPrice 
      ,[StartVolume] / 1000 as 'StartVolume'
      ,[YesterdayVolume] / 1000 as 'YesterdayVolumn'
      ,[Remark] 
      ,[AccumulatedGain] 
  FROM [StockWarehouse].[dbo].[StockPicking] sp
  join StockGroup sg on sp.Code=sg.Code
  join StockResult sr on sp.Code=sr.Code and  sr.Date='20230531'
  where (sp.Class=1 or sp.Class=5 or sp.Class=6 or sp.Class=7 or sp.Class=8 or sp.Class=9 or sp.Class=10) 　
  order by StartDate desc 
  --order by CAST(accumulatedGain as decimal) 　  　 desc 