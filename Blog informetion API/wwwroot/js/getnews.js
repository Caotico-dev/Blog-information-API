import { login, loginOut,publist } from "../utilities/utilities.js";

//nodos
const show = document.querySelector('.contenedor__noticias');

//eventos
document.addEventListener('DOMContentLoaded', getnews);
document.querySelector('#publist').addEventListener('click',(e)=>{
    e.preventDefault();
    publist();
});

document.querySelector('#cerrarsesion').addEventListener('click',(e)=>{
    e.preventDefault();
    loginOut();
});

//funciones
async function getnews(){
    const token = sessionStorage.getItem("token");
    if(token === null){
        login();
        return        
    }
    const solicitud = {
        method: "GET",
        headers: {"Authorization":`Bearer ${token}`}
    }

    try
    {
        const reponse = await fetch("/information/news",solicitud);
 
        const news = await reponse.json();
        console.log(news);
        if(news === null){
            alert("Ocurrio un error intentelo mas tarde");
            login();
            return    
        }
        shownews(news);
        
    } 
    catch(error)
    {
       login();      
        alert("Ocurrio un error intentelo mas tarde");

    }    
}

function shownews(news){

    let readnews = news;
   
    // show.querySelector("section").remove();
   
    
    for(let i = 0; i < readnews.length; i++){

        
        const news = document.createElement("section");
        news.classList.add('section');
        news.innerHTML =
        `
        <h2 class="section__titulo">${readnews[i]["titulo"]}</h2>
            <img class="section__images"  src="${readnews[i]["url_images"]}" alt="${readnews[i]["titulo"]}">
            <time class="section__time">Fecha de publicación: ${readnews[i]["fechaDePublicacion"]}</time>
            <p class="section__cuerpo">${readnews[i]["cuerpo"]}</p>        
        
        `;
        show.appendChild(news);
        
    }

}
