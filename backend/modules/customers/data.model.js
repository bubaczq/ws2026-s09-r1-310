const fs = require('fs');
const path = require('path');

const dbFile = path.join(__dirname, 'database.json');

// Adatok beolvasása a database.json fájlból
const getData = () => {
    try {
        const rawData = fs.readFileSync(dbFile);
        return JSON.parse(rawData);
    } catch (error) {
        console.error('Hiba a database.json beolvasásakor:', error);
        return { customers: [] }; // Üres tömb visszaadása hiba esetén
    }
};

module.exports = { getData };
