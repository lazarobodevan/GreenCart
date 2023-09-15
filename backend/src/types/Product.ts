import { Category } from "./Category"
import { Producer } from "./Producer"
import { Unit } from "./Unit"

export type Product = {
    id:                      String
    picture?:                Blob
    name:                    String
    description:             String
    category:                Category
    categoryId:              String
    price:                   Number
    unity:                   Unit
    unityId:                 String
    available_quantity:      Number
    is_organic:              Boolean
    harvest_date:            Date
    producer:                Producer
    producerId:              String
    createdAt:               Date
    updatedAt:               Date
    deletedAt?:              Date
}