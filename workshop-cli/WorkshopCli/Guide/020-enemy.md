[color=white]
Já tens tudo programado do teu jogador, agora falta criar os inimigos.

1. Primeiro, vamos indicaras imagens na função "load()" :
   [/color] [color=blue]
    -- Variáveis do Inimigo
    ImagemInimigo = love.graphics.newImage("meteoro.png")
    inimigos = {}
    geraInimigoTempo = 0
    geraInimigoTempoMax = 0.5
   [/color] [color=white]
2. Agora adiciona no fim da função "draw()" o seguinte:
   [/color] [color=blue]
   for index, inimigo in ipairs(inimigos) do
      love.graphics.draw(inimigo.img, inimigo.posicaoX, inimigo.posicaoY, inimigo.angulo, 1, 1, inimigo.width / 2, inimigo.height / 2)
    end
   [/color] [color=white]
3. No fim do teu código adiciona esta função:
   [/color] [color=blue]
   -- Função para atualizar os inimigos
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
               dirX = -1, -- Move-se para a esquerda
               dirY = 0,  -- Sem movimento vertical
               angulo = 0
           }
           table.insert(inimigos, inimigo)
       end
   end
   [/color] [color=white]
Esta função faz com que o inimigo apareça de certo em certo tempo (não precisas de saber mais que isto, mas se quiseres explorar estás à vontade).

4. Por fim, na função "update(dt)" depois da linha "atualizarMisseis(dt)" adiciona esta linha:
   [/color] [color=blue]
    atualizarInimigos(dt)
   [/color] [color=white]
Se executares o jogo (Alt + L) vais reparar que tens um monte de inimigos em direção à tua nave. Podes ajustar a quantidade de inimigos basta alterares o valor de "geraInimigoTempoMax".
   [/color]