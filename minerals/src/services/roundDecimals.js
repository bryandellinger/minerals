export default function roundDecimals(value, decimals) {
    return Number(Math.round(value + 'e' + decimals) + 'e-' + decimals);
}