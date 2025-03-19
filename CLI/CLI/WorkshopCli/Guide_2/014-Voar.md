[color=white]
Agora que já temos gravidade, vamos fazer com que o pássaro possa voar, 
para tal, fazemos com que ele possa subir quando pressionares a tecla "espaço".

4. Dentro da função "update()" adiciona o seguinte código:
[/color] [color=blue]
   if love.keyboard.isDown("space") then
        passaro.velocidade = passaro.salto
   end
[/color] [color=white]

Fantástico! Aquilo que fizeste agora foi configurar a tecla "espaço",
quando pressionares esta tecla o pássaro sobe.
[/color] [color=red]
     ____  _____ ____    _    _____ ___ ___  
    |  _ \| ____/ ___|  / \  |  ___|_ _/ _ \
    | | | |  _| \___ \ / _ \ | |_   | | | | |
    | |_| | |___ ___) / ___ \|  _|  | | |_| |
    |____/|_____|____/_/   \_\_|   |___\___/
[/color] [color=white]

Agora que o pássaro já consegue voar. Desafio-te a mudar a tecla que faz isso 
acontecer, por outra tecla a tua escolha.

[/color] 