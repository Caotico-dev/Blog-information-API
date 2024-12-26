import { index, loginOut, aviso } from "../utilities/utilities.js";

//Eventos
document.addEventListener('DOMContentLoaded',()=>{
    const token = sessionStorage.getItem("token");
    if(token === null){
        alert('Recuerda inicar sesion.');
        window.location.href = "login.html";
        return        
    }
});
document.querySelector('#index').addEventListener('click', (e) => {
    e.preventDefault();
    index();
});
document.querySelector('#cerrarsesion').addEventListener('click', (e) => {
    e.preventDefault();
    loginOut();
});

document.querySelector('#enviar').addEventListener('click', async () => {
    const isValid = validateAndGetFormValues();  

    if (isValid) {
        await SendForm();
        return;
    }
    aviso("Recuerda llenar todos los campos.", "Error");
});


function validateAndGetFormValues() {
   
    const titulo = document.querySelector('#titulo').value;
    const autor = document.querySelector('#autor').value;
    const fecha = document.querySelector('#fecha').value;
    const noticia = document.querySelector('#noticia').value;
    const file = document.querySelector('input[type="file"]').files[0];

    
    if (titulo !== "" && autor !== "" && fecha !== "" && noticia !== "" && file) {
       
        return { titulo, autor, fecha, noticia, file }; 
    }
 
    return false;
}

async function SendForm() {
    const formValues = validateAndGetFormValues(); 

    if (!formValues) {
        return;  
    }

    const { titulo, autor, fecha, noticia, file } = formValues;

    const form = new FormData();
    form.append('Titulo', titulo);
    form.append('Autor', autor);
    form.append('FechaDePublicacion', fecha);
    form.append('Cuerpo', noticia);
    form.append('images', file);

    const uri = "/information/news";
    const token = sessionStorage.getItem("token");
    const solicitud = {
        method: "POST",
        headers: { "Authorization": `Bearer ${token}` },
        body: form
    };

    try {
        const response = await fetch(uri, solicitud);
        const m = await response.json();
        console.log(m);
        switch (response.status) {
            case 201:
                index();
                aviso("Noticia publicada!", "Exito");
                ResetForm();
                break;
            case 400:
                aviso(response["message"], "Error");
                break;
            case 500:
                aviso("Ocurrió un error, intenta más tarde", "Error");
                break;
            default:
                ResetForm();
                break;
        }
    } catch (error) {
        index();
        alert("Ocurrió un error, inténtalo más tarde");
    }
}

function ResetForm() {
    document.querySelector('#titulo').value = "";
    document.querySelector('#autor').value = "";
    document.querySelector('#fecha').value = "";
    document.querySelector('#noticia').value = "";
    document.querySelector('input[type="file"]').value = ""; 
}
