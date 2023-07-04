
Agora vamos fazer com que os mísseis funcionem como deviam

1. Na função "load()", adiciona:

   missilVelocidade = 250
   podeDisparar = true

   2. No fim do teu código mete esta função

   function atualizarMisseis(dt)
        for i=table.getn(misseis), 1, -1 do
            missil = misseis[i]
            missil.posicaoX = missil.posicaoX + dt * missil.velocidade
        end
   end

A função "atualizarMisseis(dt)" cuida dos mísseis no jogo. Ela faz com que os mísseis se movam para a direita. É como se os mísseis estivessem a voar pelo jogo, mas desaparecendo quando vão muito longe. Assim, a função garante que os mísseis se comportem corretamente no jogo.

3. Agora vamos alterar o código que adicionamos no ultimo passo na função "update(dt)":

    if love.keyboard.isDown("space") then
        if(left) then
            missilVelocidade = missilVelocidade - velocidade/2
        elseif(right) then
            missilVelocidade = missilVelocidade + velocidade/2
        end
        if podeDisparar then
            missil = {
                posicaoX = posicaoX + 64, 
                posicaoY = posicaoY + 32, 
                width = 16, height=16, 
                velocidade = missilVelocidade, 
                imagem = missilImagem}
            table.insert(misseis, missil)
        end
    end
    atualizarMisseis(dt)

O código inserido agora faz o seguinte:
 - Se a tecla "espaço" for pressionada, um míssil é disparado com base na velocidade do jogador. A velocidade do míssil é ajustada se o jogador estiver a mover-se para a esquerda ou direita. O missil também é definido aqui.
 - A função "atualizarMisseis(dt)" é chamada para atualizar o movimento dos mísseis existentes no jogo.

Agora podes iniciar o teu jogo e divertir-te um pouco.
