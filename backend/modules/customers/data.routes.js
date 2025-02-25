const express = require('express');
const { getAllCustomers, mergeCustomers } = require('./data.controller');

const router = express.Router();

// GET - összes ügyfél lekérdezése
router.get('/', getAllCustomers);

// POST - új ügyfél hozzáadása
router.post('/', mergeCustomers);

module.exports = router;
