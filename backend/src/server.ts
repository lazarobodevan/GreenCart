import app from './app'

require('./database/index')
app.listen(8080)
console.log("Running in port 8080")