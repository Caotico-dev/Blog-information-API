export { aviso, validarEmail, index, login, publist, loginOut};


const form = document.querySelector('.form');

function aviso(mensaje,tipo){    
    const mensaj = document.createElement('div');
    mensaj.textContent = `${tipo}: ${mensaje} `;
    if(tipo === "Error"){       
        mensaj.classList.add('alert');
       
    }  
    else if(tipo === "Exito"){        
        mensaj.classList.add('success');
            
    }
    form.appendChild(mensaj);    
    setTimeout(() => {
        mensaj.remove();       
    }, 3000);
    

}
function validarEmail(email) {
    const regex = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/;
    return regex.test(email);
}
function index(){
    setTimeout(()=>{
        window.location.href = "index.html";
    },2000);
}
function login(){    

     AvisoGeneral(); 
    
    setTimeout(()=>{        
        window.location.href = "login.html";        
    },4000);
}

function loginOut(){
    setTimeout(()=>{
        sessionStorage.removeItem('token');
        setTimeout(()=>{        
            window.location.href = "login.html";        
        },4000);       
   })
}
function publist(){
    setTimeout(()=>{        
        window.location.href = "publist.html";        
    },2000);    
}
function AvisoGeneral(){
    const show = document.querySelector('.contenedor__noticias');
    const aviso = document.createElement('div');
    aviso.classList.add('alert');
    aviso.textContent = "Recuerda inicar sesion.";
    show.appendChild(aviso);
}
