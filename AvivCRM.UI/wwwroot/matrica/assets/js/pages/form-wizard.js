const step1Next=document.getElementById("step1Next"),step1Tab=document.getElementById("step1-tab"),step1=document.getElementById("step1"),step2Next=document.getElementById("step2Next"),step2Prev=document.getElementById("step2Prev"),step2Tab=document.getElementById("step2-tab"),step2=document.getElementById("step2"),step3Next=document.getElementById("step3Next"),step3Prev=document.getElementById("step3Prev"),step3Tab=document.getElementById("step3-tab"),step3=document.getElementById("step3"),step4Finish=document.getElementById("step4Finish"),step4Prev=document.getElementById("step4Prev"),step4Tab=document.getElementById("step4-tab"),step4=document.getElementById("step4");step1Next.addEventListener("click",function(){step1Tab.classList.remove("active"),step1.classList.remove("active"),step2Tab.classList.add("active"),step2.classList.add("active")}),step2Prev.addEventListener("click",function(){step1Tab.classList.add("active"),step1.classList.add("active"),step2Tab.classList.remove("active"),step2.classList.remove("active")}),step3Prev.addEventListener("click",function(){step2Tab.classList.add("active"),step2.classList.add("active"),step3Tab.classList.remove("active"),step3.classList.remove("active")}),step4Prev.addEventListener("click",function(){step3Tab.classList.add("active"),step3.classList.add("active"),step4Tab.classList.remove("active"),step4.classList.remove("active")}),step2Next.addEventListener("click",function(){step3Tab.classList.add("active"),step3.classList.add("active"),step2Tab.classList.remove("active"),step2.classList.remove("active")}),step3Next.addEventListener("click",function(){step4Tab.classList.add("active"),step4.classList.add("active"),step3Tab.classList.remove("active"),step3.classList.remove("active")});


