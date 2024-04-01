export class StringService {
    static ArrayToSentence(item:string, values:string[]):string {
        let result = "";
        const filteredValues = values.filter(f => f != item);
        for (let i = 0; i < filteredValues.length; i++) {
            const val = filteredValues[i].trim();
            if (val.length > 0) {
                // Add the separator before the value if it's not the first value
                if (result.length > 0) {
                    // Use "and" if it's the last element, otherwise use a comma
                    result += (i === values.length - 1) ? " and " : ", ";
                }
                result += val;
            }
        }

        return result;
    }
}