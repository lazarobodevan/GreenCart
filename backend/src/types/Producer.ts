import { Consumer } from "./Consumer"
import { Order } from "./Order"
import { Product } from "./Product"

export type Producer = {
    id:                 String
    email:              String
    password:           String
    origin_city:        String
    telephone:          String
    picture?:           Blob
    cpf:                String
    attend_cities:      String
    where_to_find:      String
    products:           Product[]
    orders:             Order[]
    favd_by:            Consumer[]
    createdAt:          Date 
    updatedAt:          Date 
    deletedAt?:         Date
}