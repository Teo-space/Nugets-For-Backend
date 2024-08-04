SQL

```
info: 04.08.2024 11:35:11.817 RelationalEventId.CommandExecuted[20101] (Microsoft.EntityFrameworkCore.Database.Command)
      Executed DbCommand (8ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
      SELECT COUNT(*) FROM "sqlite_master" WHERE "type" = 'table' AND "rootpage" IS NOT NULL;

info: 04.08.2024 11:35:11.905 RelationalEventId.CommandExecuted[20101] (Microsoft.EntityFrameworkCore.Database.Command)
      Executed DbCommand (0ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
      CREATE TABLE "Orders" (
          "OrderId" INTEGER NOT NULL CONSTRAINT "PK_Orders" PRIMARY KEY AUTOINCREMENT,
          "Date" TEXT NOT NULL,
          "Status" INTEGER NOT NULL
      );

info: 04.08.2024 11:35:12.011 RelationalEventId.CommandExecuted[20101] (Microsoft.EntityFrameworkCore.Database.Command)
      Executed DbCommand (3ms) [Parameters=[@p0='?' (DbType = Int32), @p1='?' (DbType = DateTime), @p2='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      INSERT INTO "Orders" ("OrderId", "Date", "Status")
      VALUES (@p0, @p1, @p2);

info: 04.08.2024 11:35:12.014 RelationalEventId.CommandExecuted[20101] (Microsoft.EntityFrameworkCore.Database.Command)
      Executed DbCommand (0ms) [Parameters=[@p0='?' (DbType = Int32), @p1='?' (DbType = DateTime), @p2='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      INSERT INTO "Orders" ("OrderId", "Date", "Status")
      VALUES (@p0, @p1, @p2);

info: 04.08.2024 11:35:12.158 RelationalEventId.CommandExecuted[20101] (Microsoft.EntityFrameworkCore.Database.Command)
      Executed DbCommand (0ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
      SELECT COUNT(*)
      FROM "Orders" AS "o"

Total: 2

info: 04.08.2024 11:35:12.182 RelationalEventId.CommandExecuted[20101] (Microsoft.EntityFrameworkCore.Database.Command)
      Executed DbCommand (0ms) [Parameters=[@__Pending_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT COUNT(*)
      FROM "Orders" AS "o"
      WHERE "o"."Status" > @__Pending_0

Pending: 1

info: 04.08.2024 11:35:12.208 RelationalEventId.CommandExecuted[20101] (Microsoft.EntityFrameworkCore.Database.Command)
      Executed DbCommand (0ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
      SELECT "o"."OrderId", "o"."Date", "o"."Status"
      FROM "Orders" AS "o"
      ORDER BY "o"."OrderId"
      LIMIT 1

Order { OrderId = 1, Date = 04.08.2024 11:35:09, Status = ќжидание }
```
