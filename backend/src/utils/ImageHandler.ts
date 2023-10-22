class ImageHandler{

    async blobToBuffer(blob: Blob){
        const buffer = await blob.arrayBuffer();
        return Buffer.from(buffer)
    }
}

export default new ImageHandler();