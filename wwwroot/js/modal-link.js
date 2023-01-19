 

import { Modal } from 'bootstrap';
  
document.addEventListener("DOMContentLoaded", function () {

    let myModalEl = document.getElementById('modal-link-dlg-box');
      
     
     
    let elements = document.querySelectorAll('.modal-link');
    elements.forEach((node) => {
        node.addEventListener("click", function () {
            let url = node.getAttribute('url');
            fetch(url)
                .then(res => res.text())
                .then(data => {
                    //console.log(data);
                    let myModalEl = document.getElementById('modal-link-dlg-box');
                    myModalEl.querySelector('.modal-body').innerHTML = data;
                    myModalEl.querySelector('.modal-title').innerHTML = node.getAttribute('title');
                    var myModal = new Modal(myModalEl);
                    myModal.show();
                }).catch(error => console.log(error));  
        });
       
    });
     
    let confirms = document.querySelectorAll(".confirm-action");
    confirms.forEach((item) => {
        item.addEventListener("click", function (e) {
            console.log(item);
            let msg = item.getAttribute("confirm-action-msg");
            msg = msg == null ? "Quer prosseguir?" : msg;

            if (!confirm(msg)) {
                e.preventDefault();
                e.stopPropagation();
                return false;
            }
            return true;
        })
    });

  
});

 