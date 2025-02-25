const dotenv = require('dotenv');

dotenv.config();

const config = {
    PORT: process.env.PORT,
    HOST: process.env.HOST,
    SESSION_SECRET: process.env.SESSION_SECRET || 'default_secret_key'
}

module.exports = { config };