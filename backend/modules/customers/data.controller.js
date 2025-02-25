const fs = require('fs');
const path = require('path');
const { v4: uuidv4 } = require('uuid'); // UUID generálás

const dbPath = path.join(__dirname, '../../data/database.json');
const tempPath = path.join(__dirname, '../../data/temp.json');

// JSON fájl beolvasása
const getData = () => {
    try {
        const jsonData = fs.readFileSync(dbPath, 'utf8');
        return JSON.parse(jsonData);
    } catch (error) {
        console.log("Nincs adat!");
        return { customers: [] }; // Ha nincs adat, üres tömböt ad vissza
    }
};

// Minden customer adat lekérése
const getAllCustomers = (req, res) => {
    const data = getData();
    res.status(200).json(data.customers);
};

// Egy adott customer keresése ID alapján
const getCustomerById = (req, res) => {
    const { id } = req.params;
    const data = getData();
    const customer = data.customers.find(c => c.id === id);

    if (!customer) {
        return res.status(404).json({ error: "A megadott azonosítóval nem található ügyfél." });
    }
    res.status(200).json(customer);
};

// JSON fájl egyesítése a meglévő adatbázissal
const mergeCustomers = (req, res) => {
    try {
        const newCustomers = req.body; // A beérkező JSON adat
        const data = getData(); // A meglévő adatbázis beolvasása

        // Ellenőrizd, hogy a beérkező adat tömb-e
        if (!Array.isArray(newCustomers)) {
            console.log("Nemaz te köcsög")
            return res.status(400).json({ error: 'A bemeneti adatnak tömbnek kell lennie.' });
        }

        // Generálj új ID-t az új ügyfeleknek és add hozzá a meglévő adatokhoz
        newCustomers.forEach(customer => {
            customer.id = uuidv4(); // Generálj új ID-t
            data.customers.push(customer); // Hozzáadás a meglévő adatokhoz
        });

        // Írd az adatokat egy ideiglenes fájlba
        fs.writeFileSync(tempPath, JSON.stringify(data, null, 2));

        // Ha sikerült, cseréljük le a meglévő fájlt
        fs.renameSync(tempPath, dbPath);

        res.status(201).json({ message: 'Az új ügyfelek sikeresen hozzáadva!', customers: newCustomers });
    } catch (error) {
        console.error(error);
        res.status(500).json({ error: 'Hiba az ügyfelek hozzáadása közben.' });
    }
};

module.exports = { getAllCustomers, getCustomerById, mergeCustomers };
