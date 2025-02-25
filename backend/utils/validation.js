const validator = require('validator');
const danger = require('./helpers');

const usernameValidation = (username) => {
    if(!username){
        throw new Error('Nincs megadva felhasználónév!');
    }
    if(danger.special.test(username)){
        throw new Error("Veszélyesek vagyunk?");
    }
};

const emailValidation = (email) => {
    if(!email){
        throw new Error('Nincs megadva email!');
    }
    if(!validator.isEmail(email) || danger.email.test(email)){
        throw new Error('Nem tudom ezt az emailt elfogadni!');
    }
};

const pswValidation = (psw) => {
    if(!psw){
        throw new Error('Nincs megadva jelszó!');
    }
    if(psw.length < 8){
        throw new Error('A jelszó túl rövid!')
    }
};

const postValidation = (post) => {
    if(!post){
        throw new Error('Nincs megadva bejegyzés!');
    }
    if(danger.special.test(post)){
        throw new Error('Veszélyesek vagyunk?');
    }
};

const versionValidation = (version) => {
    if(validator.isEmpty(version)) {
        throw new Error('A verzió mező nem lehet üres!');
    }
    if(danger.special.test(version)){
        throw new Error('Veszélyes karaktereket nem tartalmazhat a verziószám');
    }
}

module.exports = { usernameValidation, emailValidation, pswValidation, postValidation, versionValidation };