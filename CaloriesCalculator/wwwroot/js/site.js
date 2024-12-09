//const foodList = [
//    { name: 'Ябълка', caloriesPer100g: 52 },
//    { name: 'Банан', caloriesPer100g: 89 },
//    { name: 'Пица', caloriesPer100g: 266 },
//    { name: 'Пържени картофи', caloriesPer100g: 312 },
//    { name: 'Авокадо', caloriesPer100g: 160 },
//    { name: 'Ананас', caloriesPer100g: 50 },
//    { name: 'Артишок', caloriesPer100g: 47 }
//];

//let selectedFoods = [];

//function showSuggestions(query) {
//    const suggestionsDiv = document.getElementById('suggestions');
//    suggestionsDiv.innerHTML = '';

//    if (query.length === 0) {
//        suggestionsDiv.style.display = 'none';
//        return;
//    }

//    const filteredFoods = foodList.filter(food =>
//        food.name.toLowerCase().includes(query.toLowerCase())
//    );

//    if (filteredFoods.length > 0) {
//        suggestionsDiv.style.display = 'block';
//    } else {
//        suggestionsDiv.style.display = 'none';
//    }

//    filteredFoods.forEach(food => {
//        const div = document.createElement('div');
//        div.classList.add('suggestion-item');
//        div.textContent = `${food.name} - ${food.caloriesPer100g} kcal на 100g`;
//        div.onclick = () => addFood(food);
//        suggestionsDiv.appendChild(div);
//    });
//}

//function addFood(food) {
//    const foodItem = {
//        name: food.name,
//        caloriesPer100g: food.caloriesPer100g,
//        quantity: 0,
//        totalCalories: 0
//    };

//    selectedFoods.push(foodItem);
//    updateSelectedFoods();


//    const suggestionsDiv = document.getElementById('suggestions');
//    suggestionsDiv.style.display = 'none';


//    document.getElementById('searchInput').value = '';
//}




//function updateSelectedFoods() {
//    const selectedFoodsList = document.getElementById('selectedFoodsList');
//    selectedFoodsList.innerHTML = '';

//    selectedFoods.forEach((food, index) => {
//        const li = document.createElement('li');
//        li.textContent = `${food.name} - гр. `;


//        const quantityInput = document.createElement('input');
//        quantityInput.type = 'number';
//        quantityInput.placeholder = 'Колко грама?';
//        quantityInput.value = food.quantity;
//        quantityInput.oninput = (event) => {
//            food.quantity = parseFloat(event.target.value) || 0;
//        };

//        li.appendChild(quantityInput);
//        selectedFoodsList.appendChild(li);
//    });
//}

//function calculateCalories() {
//    selectedFoods.forEach(food => {
//        food.totalCalories = (food.caloriesPer100g * food.quantity) / 100;
//    });

//    updateTotalCalories();


//    document.getElementById('calculateCaloriesButton').style.display = 'none';
//    document.getElementById('startNewCalculationButton').style.display = 'inline';
//}

//function updateTotalCalories() {
//    const totalCalories = selectedFoods.reduce((sum, food) => sum + food.totalCalories, 0);
//    document.getElementById('totalCalories').textContent = totalCalories.toFixed(2);
//}

//function startNewCalculation() {

//    selectedFoods = [];
//    updateSelectedFoods();
//    document.getElementById('totalCalories').textContent = '0';
//    document.getElementById('searchInput').value = '';


//    document.getElementById('startNewCalculationButton').style.display = 'none';
//    document.getElementById('calculateCaloriesButton').style.display = 'inline';
//}