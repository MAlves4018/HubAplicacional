
    const AcertadorDeModo = () => {
        //Vai ver e o darck mode estava ou n�o ativo na ultima vez que se fechou a pagina e atualizar a p�gina de acordo.
        if (localStorage.getItem("dark-mode") === "true") {
            document.querySelector("body").classList.add("dark-mode");
            document.getElementById("dark-mode-1").classList.remove("oculto")
            document.getElementById("light-mode-on").classList.add("oculto")

        } else {
            document.querySelector("body").classList.remove("dark-mode");
            document.getElementById("light-mode-on").classList.remove("oculto")
            document.getElementById("dark-mode-1").classList.add("oculto")

        }
    }
    const AlternadorDeModo = () => {
        //Altera o css da pagina alternando entre on/off e viceversa mudando tamb�m os butoes que aparecem.
        if (document.querySelector(".dark-mode") === null) {
            document.querySelector("body").classList.add("dark-mode");
            localStorage.setItem('dark-mode', "true");
            document.getElementById("light-mode-on").classList.add("oculto")
            document.getElementById("dark-mode-1").classList.remove("oculto")
        } else {
            localStorage.setItem('dark-mode', "false");
            document.querySelector("body").classList.remove("dark-mode");
            document.getElementById("dark-mode-1").classList.add("oculto")
            document.getElementById("light-mode-on").classList.remove("oculto")
        }
    }

    const CriarCardTecnologia = (titulo, text, img, btnlink) => {
        //Criar uma card que representa uma tecnologia, esta card vai ser colocada dentro de um separador que corresponde ao tipo. 
        let div = document.createElement("div");
        div.className = "custom-card shadow ";
        div.innerHTML =
            `
            <img class="custom-card-img-top"
                src="/Image/${img}"
            alt="custom-card image" style="width:100%">
            <div class="custom-card-body">
            <h4 class="custom-card-title" >${titulo}</h4>
            <p class="custom-card-text" >${text}</p>
            <a href="${btnlink}" class="btn btn-sm btn-secondary rounded-5">ver mais</a>
            </div>
            `;
        return div;

    }

    const OcultarTodosSeparadores = () => {
        //� usado na mudan�a entre separadores para ocultar os restantes tipos e as aplica��es que fazem parte dos mesmos. 
        document.querySelectorAll(".divdeclass").forEach(ele => ele.children[0].classList.add("oculto"))
    }
    const DesocultarTodosSeparadores = () => {
        //� usado na mudan�a entre separadores para mostrar o tipo escolhido e as aplica��es que fazem parte do mesmo.
        document.querySelectorAll(".divdeclass").forEach(ele => ele.children[0].classList.remove("oculto"))
    }
    const OcultarElemento = (element) => {
        //Serve para ocultar tanto o separador do tipo como as aplica��es respetivas caso o tipo esteja disabled .
        //Para al�m disso serve para desselecionar todos os separaores e selecionar o separador mais recentemente escolhido.
        ResetarSeparadoresAtivos();
        element.classList.add("active");
        let idfunc = element.id
        if (idfunc == 'todos-tab') {
            DesocultarTodosSeparadores()
        } else {
            OcultarTodosSeparadores()
            document.querySelectorAll(".divdeclass").forEach(ele => {
                let section_id = ele.children[0].id
                if (section_id == idfunc.split('-')[0]) {
                    ele.children[0].classList.remove("oculto")
                    //ele.classList.add("active")
                }
            })
        }
    }

    const CriarSeparadorTipo = (id, name, state) => {
        //Separador correspondente ao tipo.
        separadorplacer = document.getElementById("separadorplacer")
        pageplacer = document.getElementById("pageplacer")
        let li = document.createElement("li");
        li.className = "nav-item"
        li.role = "presentation"
        //let disabled = state == true ? "" : "disabled" ///// ="" state == true ? "" : "disabled"--------------------------------------------------------------------------------------
        let disabled = state == true ? "" : "disabled"

        li.innerHTML =
            `<button class="nav-link " id="${id}-tab" data-bs-toggle="tab" 
                         data-bs-target="#${name}" type="button" role="tab" aria-controls="${name}"
                                     aria-selected="true" ${disabled} onclick="OcultarElemento(this)">
                    ${name}
                </button>`

        separadorplacer.appendChild(li);

        let diva = document.createElement("div")
        let oculto = state == true ? "" : "oculto"

        diva.className = `divdeclass ${oculto}`

        diva.innerHTML = `
            <div id="${id}" class="container tab-pane fade active show" role="tabpanel" aria-labelledby="#${name}-tab">
                <br>
                        <c class="separadorname">${name}:</c>
                        <c class="separadornumber" id="${id}paracontagem"></c>

                    <div class="container-cards" id="cards${id}">
                </div>
            </div>`

        pageplacer.appendChild(diva)
    }

    const AlertaDaCardErro = (a) => {
        //Altera o css das cards de informa��o localizadas no topo da pagina fazendo-as ficar vermelhas,
        //� utilizado para salientar tipos e aplica��es onde tenham existido dificuldades no acesso. 
        document.getElementById(a).classList.add("card-off")
    }
    const AlertaDaCardNormalizar = (a) => {
        //Altera o css das cards de informa��o localizadas no topo da pagina retirando a colora��o vermelha.
        document.getElementById(a).classList.remove("card-off")
    }

    const PopularCardNumero1 = (bons, todos) => {
        //Completa a card numero 1(Bases de dados Oracle) com o numero de tecnologias que est�o de p� pelo total de tecnologias.
        document.getElementById("1Mostrar").innerHTML = (bons + "/" + todos);
        if (bons != todos) {
            document.getElementById("1BasesdedadosORACLECard").classList.add("card-off")

        } else {
            document.getElementById("1BasesdedadosORACLECard").classList.remove("card-off")
        }
        document.getElementById("1paracontagem").innerHTML = todos
    }
    const PopularCardNumero2 = (bons, todos) => {
        //Completa a card numero 2(Bases de dados SQL Server) com o numero de tecnologias que est�o de p� pelo total de tecnologias.
        document.getElementById("2Mostrar").innerHTML = (bons + "/" + todos);
        if (bons != todos) {
            document.getElementById("2BasesdedadosSQLServerCard").classList.add("card-off")

        } else {
            document.getElementById("2BasesdedadosSQLServerCard").classList.remove("card-off")
        }
        document.getElementById("2paracontagem").innerHTML = todos
    }
    const PopularCardNumero3 = (bons, todos) => {
        //Completa a card numero 3(Servidores Aplicacionais) com o numero de tecnologias que est�o de p� pelo total de tecnologias. 
        document.getElementById("3Mostrar").innerHTML = (bons + "/" + todos);
        if (bons != todos) {
            document.getElementById("3ServidoresAplicacionaisCard").classList.add("card-off")

        } else {
            document.getElementById("3ServidoresAplicacionaisCard").classList.remove("card-off")
        }
        document.getElementById("3paracontagem").innerHTML = todos
    }
    const PopularCardNumero4 = (bons, todos) => {
        //Completa a card numero 4(Aplica��es) com o numero de tecnologias que est�o de p� pelo total de tecnologias.
        document.getElementById("4Mostrar").innerHTML = (bons + "/" + todos);
        if (bons != todos) {
            document.getElementById("4Aplicacoescard").classList.add("card-off")

        } else {
            document.getElementById("4Aplicacoescard").classList.remove("card-off")
        }

        document.getElementById("4paracontagem").innerHTML = todos

    }

    const ResetarSeparadoresAtivos = () => {
        var todososativos = document.querySelectorAll('.nav-link.active')
        todososativos.forEach(
            ele => {
                ele.classList.remove("active")
            }
        )
    }

    const TestarLigacao = (idtec) => {
        fetch(`/EstadoTecnologias/Test/${idtec}`).then(
            response => response.json()
        ).then(
            data => {
                data.forEach(
                    ele => {
                    })
            }
        ).catch(error => {
            console.log(error)
        });
    }

const PainelDeControlo = () => {
       
        var totaldeaplicacoestypeum = 0;//typeId:3 total de aplicacoes
        var respostaspositivastypeum = 0//typeId:3 respostas positivas
        var totaldeaplicacoestypetres = 0;//typeId:3 total de aplicacoes
        var respostaspositivastypetres = 0//typeId:3 respostas positivas
        var totaldeaplicacoestypequatro = 0;//typeId:4 total de aplicacoes
        var respostaspositivastypequatro = 0//typeId:4 respostas positivas
        var sqlup = 0;
        var sqltot = 0;

        fetch("/api/EstadoTecnologiasAPI").then(
            response => response.json()
        ).then(
            data => {
                data.forEach(
                    ele => {
                        if (!ele.tecnologias.apagado) {

                            switch (ele.tecnologias.typeId) {

                                case 1:
                                    if (ele.ok == true) {
                                        totaldeaplicacoestypeum++;
                                        respostaspositivastypeum++;
                                    } else {
                                        totaldeaplicacoestypeum++;
                                    }
                                    break;
                                case 2:

                                    if (ele.ok == true) {
                                        sqltot++;
                                        sqlup++;
                                    } else {
                                        sqltot++;
                                    }
                                    break;
                                case 3:
                                    if (ele.ok == true) {
                                        totaldeaplicacoestypetres++;
                                        respostaspositivastypetres++;
                                    } else {
                                        totaldeaplicacoestypetres++;
                                    }
                                    break;
                                case 4:
                                    if (ele.ok == true) {
                                        totaldeaplicacoestypequatro++;
                                        respostaspositivastypequatro++;
                                    } else {
                                        totaldeaplicacoestypequatro++;

                                    }
                                    break;
                                default:
                                    break;
                            }
                        }
                    })
                PopularCardNumero1(respostaspositivastypeum, totaldeaplicacoestypeum);
                PopularCardNumero2(sqlup, sqltot);
                PopularCardNumero3(respostaspositivastypetres, totaldeaplicacoestypetres);
                PopularCardNumero4(respostaspositivastypequatro, totaldeaplicacoestypequatro);
            }
        ).catch(error => {
            console.log(error)
        });
    }


const MetodoInicialParaFetch = () => {
    
        AcertadorDeModo();
        fetch("/api/TiposAPI").then(
            response => response.json()
        ).then(
            data => {
                //if (ele.ativo)

                    separadorplacer = document.getElementById("separadorplacer")
                separadorplacer.innerHTML = '';
                pageplacer = document.getElementById("pageplacer")
                pageplacer.innerHTML = '';
                data.forEach(
                    ele => {
                        CriarSeparadorTipo(ele.id, ele.name, ele.ativo);
                        
                    }
                )
            }
        ).catch(error => {
            console.log(error)
        });


        fetch("/api/TecnologiasAPI").then(
            response => response.json()
        ).then(
            data => {
                data.forEach(
                    ele => {
                        if (!ele.apagado) {


                            let card = CriarCardTecnologia(ele.name, ele.descricao, ele.imageName, ele.link);
                            document.getElementById(`cards${ele.tipo.id}`).appendChild(card);
                            TestarLigacao(ele.id);

                        }

                    }
                )
            }

        ).catch(error => {
            console.log(error)
        });
        PainelDeControlo();
    }

document.addEventListener("DOMContentLoaded", function () {
        MetodoInicialParaFetch();
        setInterval(PainelDeControlo, 15000);
        document.getElementById("search").addEventListener("keyup", (e) => {
            e.preventDefault();
            let arrayCards = document.querySelectorAll(".custom-card");
            arrayCards.forEach(element => {
                let cardtitle = element.querySelector(".custom-card-title");
                let cardtext = element.querySelector(".custom-card-text");

                if (!cardtitle.innerHTML.toLowerCase().includes(e.target.value.toLowerCase()) &&
                    !cardtext.innerHTML.toLowerCase().includes(e.target.value.toLowerCase())) {
                    element.classList.add("tohide")
                } else {
                    element.classList.remove("tohide")
                }
            });
        })
    });
    //const myf = (size) => {

    //    var txt = "";
    //    var intu = 1;
    //    var int = 0;
    //    do {

    //        txt += aaa.slice(int, intu);
    //        intu++;
    //        int++;
    //    } while (intu <= size)

    //    return txt;
    //}
    //const NameSlicer = (nome) => {
    //    var aaa = nome.split(' ');
    //    var nameforid = myf(aaa.length);
    //    return nameforid;
    //}
    //const topocards = (id, nome) => {
    //    var nameforid = NameSlicer(nome);
    //    var divcards = document.getElementById("all-cards");
    //    let cardinnerdiv = document.createElement("div")
    //    cardinnerdiv.classList.add("col-lg-3 col-6");
    //    let cardstatsdiv = document.createElement("div")
    //    cardstatsdiv.classList.add("stats-box");
    //    cardstatsdiv.id = id + nameforid + "Card";
    //    let titulo = document.createElement("h5")
    //    titulo.classList.add("mb-3");
    //    titulo.innerHTML = name;
    //    cardstatsdiv.appendChild(titulo);
    //    let icondiv = document.createElement("div")
    //    icondiv.classList.add("icon");
    //    let chartline = document.createElement("i")
    //    chartline.classList.add("fa-solid fa-2x fa-chart-line");
    //    icondiv.appendChild(chartline);
    //    let uptot = document.createElement("h6")
    //    uptot.innerHTML = "App up/Tot";
    //    cardstatsdiv.appendChild(uptot);
    //    divcards.appendChild(cardstatsdiv);

    //}
