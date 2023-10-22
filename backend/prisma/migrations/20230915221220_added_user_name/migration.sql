/*
  Warnings:

  - Added the required column `name` to the `Consumer` table without a default value. This is not possible if the table is not empty.
  - Added the required column `name` to the `Producer` table without a default value. This is not possible if the table is not empty.

*/
-- AlterTable
ALTER TABLE "Consumer" ADD COLUMN     "name" TEXT NOT NULL;

-- AlterTable
ALTER TABLE "Producer" ADD COLUMN     "name" TEXT NOT NULL;
