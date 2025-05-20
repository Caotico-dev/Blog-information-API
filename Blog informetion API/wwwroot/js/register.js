import { aviso, validarEmail,login} from "../utilities/utilities.js";
//nodos
const registrar = document.querySelector(".register");
const emails = document.querySelector('input[type="email"]');
const password = document.querySelector('#password');
const password_confirmacion = document.querySelector('#password_confirmacion');

//eventos
registrar.addEventListener('click',registrarse);
document.querySelector('.login').addEventListener('click',()=>{
    window.location.href = "login.html";
});

//funciones
function registrarse(){

    const email = emails.value;
    const passwords = password.value;   
    const passwords_confirmacion = password_confirmacion.value;   
   
    if(validarEmail(email))
    {
        if(passwords == passwords_confirmacion){
           
            registerData(email,passwords,passwords_confirmacion);            
            return
        }
    }
    aviso("Recuerda llenar todos los campos.","Error");
   
}
async function registerData(email,password,password_confirmacion){
    const uri = "/information/publicist";
    const data = {
        email,
        password,
        confirmPassword:password_confirmacion

    }
    const solicitud = {
        method: "POST",
        headers: {"Content-Type":"Application/json"},
        body: JSON.stringify(data)
    }
      try {
            const response = await fetch(uri, solicitud);
                   
            
            switch(response.status){
                case 200:
                    aviso(result["message"],"Exito"); 
                    login();
                    break;
                case 400:
                    const result = await response.json(); 
                    aviso(result["message"],"Exito"); 
                    break;
                    default:
                    break;
            }
        
        } catch (error) {
              
            aviso("Ocurrio un error intentelo mas tarde","Error");
        }


}


