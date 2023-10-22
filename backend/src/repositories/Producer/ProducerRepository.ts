import { Producer } from "../../types/Producer";
import ImageHandler from "../../utils/ImageHandler";
import database from "../Database";
import { IProducerRepository } from "./IProducerRepository";

export class ProducerRepository implements IProducerRepository{
    
    async save(producer: Producer): Promise<Producer> {
        const createdProducer = await database.producer.create(
            {
                data: {
                    name: producer.name,
                    email: producer.email,
                    password: producer.password,
                    cpf: producer.cpf,
                    origin_city: producer.origin_city,
                    attend_cities: producer.attend_cities,
                    telephone: producer.telephone,
                    where_to_find: producer.where_to_find,
                    picture: producer.picture || undefined,
                }
            }
        ) as Producer;

        return createdProducer;
    }
    findByEmail(email: string): Promise<Producer> {
        throw new Error("Method not implemented.");
    }

}