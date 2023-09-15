/*
  Warnings:

  - You are about to drop the `Unity` table. If the table is not empty, all the data it contains will be lost.

*/
-- DropForeignKey
ALTER TABLE "Product" DROP CONSTRAINT "Product_unityId_fkey";

-- DropTable
DROP TABLE "Unity";

-- CreateTable
CREATE TABLE "Unit" (
    "id" TEXT NOT NULL,
    "name" TEXT NOT NULL,

    CONSTRAINT "Unit_pkey" PRIMARY KEY ("id")
);

-- AddForeignKey
ALTER TABLE "Product" ADD CONSTRAINT "Product_unityId_fkey" FOREIGN KEY ("unityId") REFERENCES "Unit"("id") ON DELETE RESTRICT ON UPDATE CASCADE;
