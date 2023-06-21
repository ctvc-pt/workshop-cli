
O proximo passo é meter a nossa nave a disparar mísseis e tal como 
a nave o primeiro passo é indicar a imagem e os seus atributos na 
função "load()".

1. Primeiro escolhe na pasta uma imagem para ser o míssel da tua nave.
Adiciona o seguinte código na função "load()"

   missilImagem = love.graphics.newImage("missil1.png")
   misseis = {}

   2. Agora vamos desenhar os mísseis na função "draw()"

   for index, missil in ipairs(misseis) do
       love.graphics.draw(missil.imagem, missil.x, missil.y)
       end
   end


3. Insere no update(dt) o seguinte codigo:

    if love.keyboard.isDown("b") then
        missil = {x = posicaoX, y = posicaoY, velocidade = 300, imagem = missilImagem}
        table.insert(misseis, missil)
    end

Ao precionar a tecla 'b' cria e lança um míssil no jogo. 

4. Inicia o jogo (alt+L) e vê o que há de novo.

