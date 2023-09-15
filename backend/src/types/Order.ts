import { Consumer } from "./Consumer"
import { Producer } from "./Producer"
import { Product } from "./Product"
import { Status } from "./Status"

export type Order = {
    id:                      String
    consumer:                Consumer
    producer:                Producer
    product:                 Product
    quantity:                Number
    consumer_obs?:           String
    producer_obs?:           String
    status:                  Status
  
    createdAt:               Date
    acceptedAt?:             Date
    rejectedAt?:             Date
    updatedAt:               Date
    deletedAt?:              Date
}