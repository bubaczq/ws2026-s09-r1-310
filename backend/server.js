const app = require('./app');
const {PORT,HOST} = require('./config/config').config;

app.listen(PORT,HOST, ()=>{
    console.log(`http://${HOST}:${PORT}`);
});