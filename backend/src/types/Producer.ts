import { Consumer } from "./Consumer"
import { Order } from "./Order"
import { Product } from "./Product"

export type Producer = {
    id?:                 string
    name:               string
    email:              string
    password:           string
    origin_city:        string
    telephone:          string
    picture?:           Buffer
    cpf:                string
    attend_cities:      string
    where_to_find:      string
    products:           Product[]
    orders:             Order[]
    favd_by:            Consumer[]
    createdAt?:         Date 
    updatedAt?:         Date 
    deletedAt?:         Date
}