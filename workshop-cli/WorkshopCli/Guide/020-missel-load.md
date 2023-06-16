
O proximo passo é meter a nossa nave a disparar mísseis e tal como 
a nave o primeiro passo é indicar a imagem e os seus atributos na 
função "load()".

1. Primeiro escolhe na pasta uma imagem para ser o míssel da tua nave.
Implementa o seguinte código na função "load()"

    missilImagem = love.graphics.newImage("nome_imagem.png")
    missilTempoMax = 0.2
    missilVelocidade = 250
    misseis = {}
    podeDisparar = true
    missilTempo = missilTempoMax

2. Agora vamos desenhar os mísseis na função "draw()"

for index, missil in ipairs(misseis) do
    love.graphics.draw(missil.imagem, missil.posicaoX, missil.posicaoY)
  end

3. Insere no final do código:

function gerarMissil(x, y, missilVelocidade)
  if podeDisparar then
    missil = {posicaoX = x, posicaoY = y, width = 16, height=16, velocidade = missilVelocidade, imagem = missilImagem}
    table.insert(misseis, missil)

    podeDisparar = false
    missilTempo = missilTempoMax
  end
end

A função "gerarMissil(x, y, missilVelocidade)" cria e lança um míssil no jogo. Ela verifica se podemos disparar um míssil e, se sim, cria um novo míssil com uma posição inicial, tamanho, velocidade e imagem definidos. O míssil é adicionado ao jogo e impedimos que mais mísseis sejam disparados imediatamente. Depois de algum tempo, poderemos disparar outro míssil. Assim, a função controla o lançamento de mísseis no jogo.

4. Inicia o jogo (alt+L) e vê o que há de novo.

