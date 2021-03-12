export class UIKitUtilities {
/**
 * Will Scroll to selector could be either an Id(#example) or Class(.example).
 * Change the indexOfSelector if you want to go the nth occurrence.
 * 
 * @static
 * @param {string} selector 
 * @param {number} [indexOfSelector=0] 
 * @memberof UIKitUtilities
 */
public static scrollIntoView(selector: string, indexOfSelector = 0): void {
        const elements = document.querySelectorAll(selector);
        if (elements != null && elements.length > 0) {
            const el = elements[indexOfSelector] as HTMLElement;
            el.scrollIntoView({ behavior: 'smooth', block: 'start' });
        }
    }
}
