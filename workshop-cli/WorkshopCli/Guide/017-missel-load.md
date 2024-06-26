
O próximo passo é meter a nossa nave a disparar mísseis e, tal como fizemos para adicionar a nave ao jogo o primeiro passo é carregar e desenhar a imagem.

1. Primeiro escolhe dentro da pasta "mygame" uma imagem para ser o míssil da tua nave. 

Adiciona o seguinte código na função "load()" antes da palavra "end":

   -- Variáveis do Míssil
    missilImagem = love.graphics.newImage("missil1.png")
    misseis = {}

2. Agora vamos desenhar os mísseis na função "draw()"

    for index, missil in ipairs(misseis) do
       love.graphics.draw(missil.imagem, missil.posicaoX, missil.posicaoY)
    end

3. Insere na função update(dt) o seguinte código:

   -- Disparo do míssil
    if love.keyboard.isDown("space") then
        missil = {posicaoX = posicaoX, posicaoY = posicaoY, velocidade = 300, imagem = missilImagem}
        table.insert(misseis, missil)
    end

Ao pressionar a tecla 'SPACEBAR' a nave dispara um míssil. 

4. Inicia o jogo e vê o que há de novo.

Os mísseis não saem do sítio, certo? É isso que vamos fazer a seguir.

