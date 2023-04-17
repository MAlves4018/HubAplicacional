
// JS Dependencies: Popper, Bootstrap & JQuery
 
import * as $ from 'jquery';
window.$ = $;


import 'jquery-ui-bundle';
import 'jquery-validation';
import 'jquery-validation-unobtrusive';
 
import '@fortawesome/fontawesome-free/js/all.js';
import '@fortawesome/fontawesome-free/css/all.css';

import '@popperjs/core';
import 'bootstrap'; 
import 'select2' 

import '../json/datatable-pt-pt.json';

// CSS Dependencies: Bootstrap
import 'bootstrap/dist/css/bootstrap.min.css'; 

import 'datatables.net';
import 'datatables.net-dt/css/jquery.dataTables.css';



// Custom JS imports
// ... none at the moment
import '../js/accordion.js';
import '../js/bootstrap-toaster.js';
import '../js/menu-tree.js';  
import '../js/application.js'; 
import '../js/modal-link.js';
import '../js/monitorizacao.js';

  
// Custom CSS imports
import '../css/site.css';
import '../css/accordion.css';
import '../css/bootstrap-toaster.css';
import '../css/menu-tree.css';
import '../css/global.css';
import '../css/modal-link.css';
import '../css/monitorizacao.css';

console.log('The \'site\' bundle has been loaded!');





//$(function () {
//    if (typeof (siteModelErrors) !== undefined && siteModelErrors !== null)
//        siteModelErrors.forEach(function (child, index) {
//            Toast.create(child['Title'], child['Message'], child['Error'], child['Timeout']);
//        });
//});

