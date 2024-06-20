
O proximo passo é meter a nossa nave a disparar mísseis e tal como a nave o primeiro passo é indicar a imagem e os seus atributos na função "load()".

Primeiro escolhe na pasta uma imagem para ser o míssel da tua nave.
Adiciona o seguinte código na função "load()"

<span style="color:cyan">
missilImagem = love.graphics.newImage("missil1.png")
misseis = {}
</span>
Agora vamos desenhar os mísseis na função "draw()"

<span style="color:cyan">for index, missil in ipairs(misseis) do
       love.graphics.draw(missil.imagem, missil.posicaoX, missil.posicaoY)
    end

Isto vai armazenar os misseis todos para que depois podemos controlá-los.


Insere no update(dt) o seguinte codigo:

<span style="color:cyan">
if love.keyboard.isDown("space") then
missil = {posicaoX = posicaoX, posicaoY = posicaoY, velocidade = 300, imagem = missilImagem}
table.insert(misseis, missil)
end


Ao pressionar a tecla 'SPACEBAR' cria e lança um míssil no jogo. 

4. Inicia o jogo (alt+L) e vê o que há de novo.

