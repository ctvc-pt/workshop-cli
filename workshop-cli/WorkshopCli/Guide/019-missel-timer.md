
Agora vamos tornar os misseis mais realistas, vamos criar um temporizador, que é utilizado para controlar o intervalo entre os disparos dos mísseis. Quando o temporizador termina, é possível disparar outro míssil.

1. Na função "load()" adiciona as variaveis do missil:

   missilTempoMax = 0.2
   missilTempo = missilTempoMax

2. Na função "update(dt)", depois da linha "if podeDisparar then" adiciona:

   podeDisparar = false
   missilTempo = missilTempoMax

3. Também na função "update(dt)", e antes da linha "atualizarMisseis(dt)", coloca:

   if missilTempo > 0 then
     missilTempo = missilTempo - dt
   else
     podeDisparar = true
   end

Basicamente, sempre que "missilTempo" for maior que 0 e mais pequeno que o tempo máximo (missilTempoMax), a opção de podeDisparar está desativa, e vice-versa.
