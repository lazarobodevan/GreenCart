import { Order } from "./Order"
import { Producer } from "./Producer"

export type Consumer = {
    id:              String
    email:           String
    password:        String
    origin_city:     String
    telephone:       String
    picture?:        Blob
    cpf:             String
    orders:          Order[]
    fav_producers:   Producer[]
    createdAt:       Date
    updatedAt:       Date
    deletedAt?:      Date
}