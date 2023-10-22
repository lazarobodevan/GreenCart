import { randomUUID } from "crypto";
import { Producer } from "../../../../src/types/Producer";
import { Order } from "../../../../src/types/Order";
import { Product } from "../../../../src/types/Product";
import { ProducerRepository } from "../../../../src/repositories/Producer/ProducerRepository";

describe("#Producer Repository", ()=>{
    
    it("Should create a new Producer", async ()=>{
        //arrage
        const producer = {
            name: "Producer",
            email:"test@test.com",
            password: "123456",
            cpf:"111.111.111-11",
            origin_city: "A",
            attend_cities: "A,B,C,D",
            orders:[],
            products: [],
            telephone:"(11)91111-1111",
            where_to_find: "Local farm",
            favd_by:[],
        } as Producer;

        const producerRepository = new ProducerRepository();
        
        jest.mock("../../../../src/repositories/Producer/ProducerRepository.ts");

        //act
        const createdProducer = await producerRepository.save(producer);    

        //assert
        expect(createdProducer).toEqual(expect.objectContaining(createdProducer));
    });
})