
Agora temos tudo sobre o jogador feito, passaremos para os inimigos.

1. Vamos começar por indicar as imagens as suas variaveis na função "load()" (podes mudar a imagem a teu gosto):

    -- Variáveis do Inimigo
    ImagemInimigo = love.graphics.newImage("meteoro.png")
    inimigos = {}
    geraInimigoTempo = 0
    geraInimigoTempoMax = 0.5

2. Agora vamos editar na função "draw()", adiciona no fim da função o seguinte:

   for index, inimigo in ipairs(inimigos) do
      love.graphics.draw(inimigo.img, inimigo.posicaoX, inimigo.posicaoY, inimigo.angulo, 1, 1, inimigo.width / 2, inimigo.height / 2)
    end

3. No fim do teu código adiciona esta função:

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
       for i = #inimigos, 1, -1 do
           local inimigo = inimigos[i]
           if inimigo.dirX and inimigo.dirY then
               -- Move o inimigo na direção calculada
               inimigo.posicaoX = inimigo.posicaoX + inimigo.dirX * inimigo.velocidade * dt
               inimigo.posicaoY = inimigo.posicaoY + inimigo.dirY * inimigo.velocidade * dt
           else
               inimigo.posicaoX = inimigo.posicaoX - inimigo.velocidade * dt
           end

           if inimigo.posicaoX < -inimigo.width or inimigo.posicaoX > love.graphics.getWidth() or
              inimigo.posicaoY < -inimigo.height or inimigo.posicaoY > love.graphics.getHeight() then
               table.remove(inimigos, i)
           end
       end
   end

Esta função faz com que o inimigo apareça de certo em certo tempo(não precisas de saber mais que isto, mas se quiseres explorar estás à vontade).

4. Por fim na função "update(dt)" depois da linha "atualizarMisseis(dt)" adiciona esta linha:

    atualizarInimigos(dt) 

Agora deves ter um monte de inimigos a vir em direção da tua nave(alt+L). Altera o valor de "geraInimigoTempoMax" para teres à tua escolha a quantidade de inimigos que vai aparecendo ao longo do tempo.

    