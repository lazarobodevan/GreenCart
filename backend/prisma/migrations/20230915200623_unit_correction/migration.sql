/*
  Warnings:

  - Made the column `picture` on table `Producer` required. This step will fail if there are existing NULL values in that column.
  - Made the column `attend_cities` on table `Producer` required. This step will fail if there are existing NULL values in that column.
  - Made the column `where_to_find` on table `Producer` required. This step will fail if there are existing NULL values in that column.

*/
-- AlterTable
ALTER TABLE "Producer" ALTER COLUMN "picture" SET NOT NULL,
ALTER COLUMN "attend_cities" SET NOT NULL,
ALTER COLUMN "where_to_find" SET NOT NULL;
