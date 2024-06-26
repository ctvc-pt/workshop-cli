
Agora, vamos adicionar um temporizador para controlar o intervalo entre os disparos dos mísseis, assim os disparos ficam mais realistas!

1. Na função "load()" adiciona as variáveis do míssil:

    missilTempoMax = 0.2
    missilTempo = missilTempoMax

2. Na função "update(dt)", depois da linha "if podeDisparar then" adiciona:

     podeDisparar = false
     missilTempo = missilTempoMax

3. Também na função "update(dt)", e antes da linha "atualizarMisseis(dt)", adiciona:

   if missilTempo > 0 then
     missilTempo = missilTempo - dt
   else
     podeDisparar = true
   end

