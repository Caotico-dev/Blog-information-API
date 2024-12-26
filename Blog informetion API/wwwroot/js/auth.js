import { aviso,validarEmail, index} from "../utilities/utilities.js";
//Nodos
const token = document.addEventListener('DOMContentLoaded', sesion);
const iniciarSesion = document.querySelector(".btn");
const emails = document.querySelector('input[type="email"]');
const passwords = document.querySelector('input[type="password"]');


//Eventos
document.querySelector('.register').addEventListener('click',()=>{
    window.location.href = "register.html";    
});

iniciarSesion.addEventListener('click',autenticarse);

//Funciones
function sesion(){
    const get = sessionStorage.key(token);
    if(get !== null)
    {
        index();
        alert("Tienes que cerrar la sesion antes de volver al inicio.");        
    }
}
function autenticarse(){

    const email = emails.value;
    const password = passwords.value;   
    
    if(email === "" && password === ""){
        
        aviso("Recuerda llenar todos los campos.","Error");
        return
    }
    if(validarEmail(email)){
        validarDatos(email,password);        
        return         
    }
    aviso("Pon un email valido","Error");
    limpiarCampos();      
}
function limpiarCampos(){
    emails.value = "";
    passwords.value = "";
}

async function validarDatos(email, password) {   


    const uri = "/information/publicists";
    const data = {
        email,
        password
    };

    const solicitud = {
        method: "POST",
        headers: { "Content-Type": "application/json" },  
        body: JSON.stringify(data),             
    };

    try {
        const response = await fetch(uri, solicitud);  

        switch(response.status){
            case 200:
                aviso("Bienvenido!","Exito");
                const result = await response.json(); 
                sessionStorage.setItem("token",result["token"]["result"]);    
                index();         
            break;
            case 401:
                aviso("Contraseña o correo incorretos.","Error");
                break; 
            case 500:
                aviso("Ocurrio un error intenta mas tarde","Error");  
                break;
                default:
                    break;  
        }        

        
    } catch (error) {

        aviso("Ocurrio un error intentalo mas tarde","Error");
    }
}



