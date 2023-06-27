
Agora temos tudo sobre o jogador feito, passaremos para os inimigos.

1. Vamos começar por indicar as imagens as suas variaveis na função "load()" (podes mudar a imagem a teu gosto):

 ImagemInimigo = love.graphics.newImage("inimigo1.png")
 inimigos = {}
 geraInimigoTempo = 0
 geraInimigoTempoMax = 0.5

2. Agora vamos editar na função "draw()", adiciona no fim da função o seguinte:

    for index, inimigo in ipairs(inimigos) do
        love.graphics.draw(inimigo.img, inimigo.posicaoX, inimigo.posicaoY)
    end

3. No fim do teu código adiciona esta função:

    function atualizarInimigos(dt)
         geraInimigoTempo = geraInimigoTempo - dt
         if geraInimigoTempo <= 0 then
             geraInimigoTempo = geraInimigoTempoMax
             y = love.math.random(0, love.graphics.getHeight() - 64)
             inimigo = {posicaoX = love.graphics.getWidth(), posicaoY = y, width = 64, height = 64, velocidade = 100, img = ImagemInimigo}
             table.insert(inimigos, inimigo)
         end
         for i = #inimigos, 1, -1 do
             inimigo = inimigos[i]
             inimigo.posicaoX = inimigo.posicaoX - inimigo.velocidade * dt
         end
    end

Esta função faz com que o inimigo apareça de certo em certo tempo.

4. Por fim na função "update(dt)" depois da linha "atualizarMisseis(dt)" adiciona esta linha:

    atualizarInimigos(dt) 

Agora deves ter um monte de inimigos a vir em direção da tua nave(alt+L). Altera o valor de "geraInimigoTempoMax" para teres à tua escolha a quantidade de inimigos que vai aparecendo ao longo do tempo.

    