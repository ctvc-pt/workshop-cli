
Agora vamos tornar os misseis mais realistas, vamos criar um temporizador, que é utilizado para controlar o intervalo entre os disparos dos mísseis. Quando o temporizador termina, é possível disparar outro míssil.

Na função "load()" adiciona:
<span style="color:cyan">missilTempoMax = 0.2
   missilTempo = missilTempoMax

Na função "update(dt)", depois da linha "if podeDisparar then" adiciona:
<span style="color:cyan">podeDisparar = false
   missilTempo = missilTempoMax

Também na função "update(dt)", e antes da linha "atualizarMisseis(dt)", coloca:
<span style="color:cyan">if missilTempo > 0 then
     missilTempo = missilTempo - dt
   else
     podeDisparar = true
   end

Basicamente, sempre que "missilTempo" for maior que 0 e mais pequeno que o tempo máximo (missilTempoMax), a opção de podeDisparar está desativa, e vice-versa.

