[color=white]
Agora, vamos adicionar um temporizador para controlar o intervalo
entre os disparos dos mísseis, assim ficam mais realistas!

1. Na função "load()" adiciona as variáveis do míssil:
   [/color] [color=blue]
    missilTempoMax = 0.2
    missilTempo = missilTempoMax
   [/color] [color=white]
2. Na função "update(dt)", depois da linha "if podeDisparar then" adiciona:
   [/color] [color=blue]
     podeDisparar = false
     missilTempo = missilTempoMax
   [/color] [color=white]
3. Também na função "update(dt)", e antes da linha "atualizarMisseis(dt)", 
adiciona:
   [/color] [color=blue]
   if missilTempo > 0 then
     missilTempo = missilTempo - dt
   else
     podeDisparar = true
   end
   [/color]
