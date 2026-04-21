[color=white]
Já tens tudo programado para tua nave, agora falta criar os inimigos.

1. Primeiro, vamos indicar as imagens na função "load()" :
   [/color] [color=blue]
    ImagemInimigo = love.graphics.newImage("meteoro.png")
    inimigos = {}
    geraInimigoTempo = 0
    geraInimigoTempoMax = 0.5
   [/color] [color=white]
2. Adiciona no fim da função "draw()" o seguinte:
   [/color] [color=blue]
   for index, inimigo in ipairs(inimigos) do
      love.graphics.draw(inimigo.img, inimigo.posicaoX, inimigo.posicaoY, inimigo.angulo, 1, 1, inimigo.width / 2, inimigo.height / 2)
    end
   [/color] [color=white]
3. No fim do teu código adiciona esta função:
   [/color] [color=blue]
   function atualizarInimigos(dt)
       geraInimigoTempo = geraInimigoTempo - dt
       if geraInimigoTempo <= 0 then
           geraInimigoTempo = geraInimigoTempoMax
           local y = love.math.random(0, love.graphics.getHeight() - ImagemInimigo:getHeight())
           local inimigo = {
               posicaoX = love.graphics.getWidth(),
               posicaoY = y,
               width = ImagemInimigo:getWidth(),
               height = ImagemInimigo:getHeight(),
               velocidade = 100,
               img = ImagemInimigo,
               dirX = -1, 
               dirY = 0,  
               angulo = 0
           }
           table.insert(inimigos, inimigo)
       end
   end
   [/color] [color=white]
Como funciona:
- "geraInimigoTempo" é um contador que baixa a cada frame 
  ("- dt"). Quando chega a zero, criamos um novo inimigo e o 
  contador volta a "geraInimigoTempoMax".
- "love.math.random(0, ...)" escolhe uma posição Y aleatória, 
  para que os inimigos apareçam em alturas diferentes.
- "inimigo.posicaoX = love.graphics.getWidth()" faz com que o 
  inimigo apareça na borda direita do ecrã.
- "table.insert(inimigos, inimigo)" adiciona o novo inimigo à 
  lista, tal como fazemos com os mísseis.

4. Por último, na função "update(dt)" depois da linha "atualizarMisseis(dt)" 
adiciona esta linha:
   [/color] [color=blue]
    atualizarInimigos(dt)
   [/color] [color=white]
Se executares o jogo (Alt + L) vais reparar que tens um monte de 
inimigos em direção à tua nave. Podes ajustar a quantidade de 
inimigos basta alterares o valor de "geraInimigoTempoMax".
   [/color]