var tabledata=[{id:1,name:"Oli Bob",progress:12,gender:"male",rating:1,col:"red",dob:"19/02/1984",car:1},{id:2,name:"Mary May",progress:1,gender:"female",rating:2,col:"blue",dob:"14/05/1982",car:!0},{id:3,name:"Christine Lobowski",progress:42,gender:"female",rating:0,col:"green",dob:"22/05/1982",car:"true"},{id:4,name:"Brendon Philips",progress:100,gender:"male",rating:1,col:"orange",dob:"01/08/1980"},{id:5,name:"Margret Marmajuke",progress:16,gender:"female",rating:5,col:"yellow",dob:"31/01/1999"},{id:6,name:"Frank Harbours",progress:38,gender:"male",rating:4,col:"red",dob:"12/05/1966",car:1},{id:1,name:"Oli Bob",progress:12,gender:"male",rating:1,col:"red",dob:"19/02/1984",car:1},{id:2,name:"Mary May",progress:1,gender:"female",rating:2,col:"blue",dob:"14/05/1982",car:!0},{id:3,name:"Christine Lobowski",progress:42,gender:"female",rating:0,col:"green",dob:"22/05/1982",car:"true"},{id:4,name:"Brendon Philips",progress:100,gender:"male",rating:1,col:"orange",dob:"01/08/1980"},{id:5,name:"Margret Marmajuke",progress:16,gender:"female",rating:5,col:"yellow",dob:"31/01/1999"},{id:6,name:"Frank Harbours",progress:38,gender:"male",rating:4,col:"red",dob:"12/05/1966",car:1}],dateEditor=function(e,t,r,a){var n=moment(e.getValue(),"DD/MM/YYYY").format("YYYY-MM-DD"),o=document.createElement("input");function i(){o.value!=n?r(moment(o.value,"YYYY-MM-DD").format("DD/MM/YYYY")):a()}return o.setAttribute("type","date"),o.style.padding="4px",o.style.width="100%",o.style.boxSizing="border-box",o.value=n,t(function(){o.focus(),o.style.height="100%"}),o.addEventListener("blur",i),o.addEventListener("keydown",function(e){13==e.keyCode&&i(),27==e.keyCode&&a()}),o},table=new Tabulator("#editable",{height:"310px",layout:"fitColumns",reactiveData:!0,data:tabledata,columns:[{title:"Name",field:"name",width:150,editor:"input"},{title:"Location",field:"location",width:130,editor:"autocomplete",editorParams:{allowEmpty:!0,showListOnEmpty:!0,values:!0}},{title:"Progress",field:"progress",sorter:"number",hozAlign:"left",formatter:"progress",width:140,editor:!0},{title:"Gender",field:"gender",editor:"select",editorParams:{values:{male:"Male",female:"Female",unknown:"Unknown"}}},{title:"Rating",field:"rating",formatter:"star",hozAlign:"center",width:100,editor:!0},{title:"Date Of Birth",field:"dob",hozAlign:"center",sorter:"date",width:140,editor:dateEditor},{title:"Driver",field:"car",hozAlign:"center",editor:!0,formatter:"tickCross"}]});document.getElementById("reactivity-add").addEventListener("click",function(){tabledata.push({name:"IM A NEW ROW",progress:100,gender:"male"})}),document.getElementById("reactivity-delete").addEventListener("click",function(){tabledata.pop()}),document.getElementById("reactivity-update").addEventListener("click",function(){tabledata[0].name="IVE BEEN UPDATED"});


