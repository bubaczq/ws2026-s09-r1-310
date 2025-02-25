const express = require('express');
const cookieParser = require('cookie-parser');
const session = require('express-session');
const cors = require('cors');
const path = require('path');
const { config } = require('./config/config');

const customerRoutes = require('./modules/customers/data.routes'); 


const app = express();

app.use(express.json());
app.use(express.urlencoded({extended: true}));

app.use('/api/customers', customerRoutes);

module.exports = app;